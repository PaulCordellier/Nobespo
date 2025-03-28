<script setup lang="ts">
import { ref } from "vue"
import { useRouter } from "vue-router"
import onLongTextAreaInput from "@/misc/onLongTextAreaInput"
import LoadingWrapper from "@/components/LoadingWrapper.vue"
import { useCurrentUserStore } from "@/stores/currentUser"
import FieldVerifier from "@/components/FieldVerifier.vue"
import { type FieldVerifierInfo } from "@/components/FieldVerifier.vue"
import ButtonWithLoading from "@/components/ButtonWithLoading.vue"
import { ResponseState } from "@/components/ButtonWithLoading.vue"
import ListOfFilmsAndSeries from "@/components/ListOfFilmsAndSeries.vue"
import SearchBar from "@/components/SearchBar.vue"
import { type Watchlist } from "@/misc/watchlist-types"

const currentUserStore = useCurrentUserStore()
const router = useRouter()

const watchlist = ref<Watchlist>({
    title: "",
    description: "",
    filmsIds: [],
    seriesIds: []
})

const showSearchResults = ref(false)
const responseState = ref<ResponseState>(ResponseState.NoRequest)
const searchResults = ref<any[] | undefined>([])    
const loadingErrorMessage = ref<string>()
const addedMedias = ref<any[]>([])
const filedVerifiersWithErrorIcon = ref<boolean>(false)

async function addWatchlist() {

    filedVerifiersWithErrorIcon.value = true
    
    if (fieldVerifierInfos.some(x => !x.isValid())) {
        return
    }

    responseState.value = ResponseState.Loading

    const response = await fetch('/api/watchlist/add', {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${currentUserStore.token}`,
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(watchlist.value)
        }
    )

    if (response.ok) {
        router.push('/')    // TODO go to a more appropriate route
    } else if (response.status == 401 || (response.body && await response.json() == 'Bad token')) {
        currentUserStore.disconnectUser()
        router.push({ name: 'login' })
    } else {
        responseState.value = ResponseState.Error
    }
}

async function fetchSearchResults(searchQuery : string) {

    searchResults.value = undefined
    loadingErrorMessage.value = undefined
    showSearchResults.value = true

    if (searchQuery == "") {
        searchResults.value = []
        return
    }

    searchQuery = encodeURI(searchQuery)

    const response = await fetch(`/api/tmdb/search-films-and-series/${searchQuery}`, { method: "GET" })

    if (response.ok) {
        const apiResponse = await response.json()
        searchResults.value = apiResponse.results as any[]
        searchResults.value = searchResults.value.slice(0, 5)
    } else {
        loadingErrorMessage.value = "Fehler: Code " + response.status
    }
}

const fieldVerifierInfos : FieldVerifierInfo[] = [
    {
        isValid: () => watchlist.value.title.trim().length > 0,
        description: "Der Titel ist obligatorisch."
    },
    {
        isValid: () => watchlist.value.filmsIds.length + watchlist.value.seriesIds.length >= 2,
        description: "Eine Watchlist muss mindestens zwei Filme oder Serien enthalten."
    }
]

function addMedia(mediaIndex : number, event : Event) {
    event.preventDefault()

    let mediaToAdd = searchResults.value![mediaIndex]
    searchResults.value!.splice(mediaIndex, 1)

    addedMedias.value.push(mediaToAdd)

    if (mediaToAdd.media_type == 'movie') {
        watchlist.value.filmsIds.push(mediaToAdd.id)

    } else if (mediaToAdd.media_type == 'tv') {
        watchlist.value.seriesIds.push(mediaToAdd.id)

    } else {
        console.error(`Can't understand the value media_type '${mediaToAdd.media_type}'`)
    }
}

function removeMedia(mediaIndex : number, event : Event) {
    event.preventDefault()

    let mediaToRemove = addedMedias.value[mediaIndex]

    addedMedias.value.splice(mediaIndex, 1)

    if (mediaToRemove.media_type == 'movie') {
        watchlist.value.filmsIds.splice(mediaToRemove.id, 1)

    } else if (mediaToRemove.media_type == 'tv') {
        watchlist.value.seriesIds.splice(mediaToRemove.id, 1)

    } else {
        console.error(`Can't understand the value media_type '${mediaToRemove.media_type}'`)
    }
}

function onSearchBarTextRemoved() {
    searchResults.value = undefined
    showSearchResults.value = false
    loadingErrorMessage.value = undefined
}
</script>

<template>
    <div class="basic-margin-container">
        <p class="text-field-label">Titel:</p>
        <input
            type="text"
            class="text-field"
            v-model="watchlist.title"
            maxlength="200"
        >

        <p class="text-field-label">Beschreibung:</p>
        <textarea
            class="long-text-area"
            maxlength="10000"
            @input="onLongTextAreaInput($event.target as HTMLTextAreaElement)"
            v-model="watchlist.description"
        ></textarea>

        <p v-if="addedMedias.length > 0" class="text-field-label">Filme und Serien auf der Watchlist:</p>
        <ListOfFilmsAndSeries
            :medias="addedMedias"
            :button-info="{ buttonText: 'Löschen', buttonEvent: removeMedia }"
            :should-be-small="true"
        />

        <SearchBar
            placeholder="Nach Filmen oder Serien suchen, um sie auf die Watchlist zu setzen"
            :onSearchTextRemoved="onSearchBarTextRemoved"
            :actionOnEnterPressed="fetchSearchResults"
        />

        <LoadingWrapper v-if="showSearchResults" :loaded-ref="searchResults" :error-message="loadingErrorMessage">
            <p v-if="searchResults!.length > 0" class="text-field-label">Suchergebnisse:</p>
            <p v-else-if="searchResults">Kein Ergebnis</p>
            <ListOfFilmsAndSeries
                :medias="searchResults"
                :button-info="{ buttonText: 'Hinzufügen', buttonEvent: addMedia }"
                :should-be-small="true"
            />
        </LoadingWrapper>

        <div v-for="fieldVerifierInfo in fieldVerifierInfos">
            <FieldVerifier
                :is-valid="fieldVerifierInfo.isValid()"
                :description="fieldVerifierInfo.description"
                :hideWhenNoError="true"
                :withErrorIcon="filedVerifiersWithErrorIcon"
            />
        </div>

        <ButtonWithLoading button-text="Erstellen" :button-event="addWatchlist" :responseState="responseState" />
    </div>
</template>
