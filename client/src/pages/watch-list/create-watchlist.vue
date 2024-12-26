<script setup lang="ts">
import { ref, useTemplateRef } from "vue"
import { useRouter } from "vue-router"
import onLongTextAreaInput from "@/misc/onLongTextAreaInput"
import LoadingWrapper from "@/components/LoadingWrapper.vue"
import { MdRipple } from "@material/web/ripple/ripple"
import { useCurrentUserStore } from "@/stores/currentUser"
import FieldVerifier from "@/components/FieldVerifier.vue"
import { type FieldVerifierInfo } from "@/components/FieldVerifier.vue"
import ButtonWithLoading from "@/components/ButtonWithLoading.vue"
import { ResponseState } from "@/components/ButtonWithLoading.vue"

const currentUserStore = useCurrentUserStore()
const showButtonToRemoveSearchText = ref<boolean>(false)
const searchBar = useTemplateRef('watchlist-search-bar')
const router = useRouter()

type Watchlist = {
    title: string
    description: string
    filmsIds: number[]
    seriesIds: number[]
}

const watchlist = ref<Watchlist>({
    title: "",
    description: "",
    filmsIds: [],
    seriesIds: []
})

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

async function fetchFilmsAndSeries(searchQuery : string) {

    searchResults.value = undefined
    loadingErrorMessage.value = undefined

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
        isValid: () => watchlist.value.filmsIds.length > 0 || watchlist.value.seriesIds.length > 0,
        description: "Es gibt keine Serien oder Filme auf Ihrer Watchlist."
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

function onTitleInput(event: Event) {
    watchlist.value.title = (event.target as HTMLTextAreaElement).value
}

function onDescriptionAreaInput(event: Event) {
    let descriptionAreaInput = event.target as HTMLTextAreaElement
    onLongTextAreaInput(descriptionAreaInput)
    watchlist.value.description = descriptionAreaInput.value
}

function onSearchBarInput(event: Event) {
    let searchBar = event.target as HTMLInputElement
    showButtonToRemoveSearchText.value = searchBar.value != ''
    if (searchBar.value == '') {
        searchResults.value = []
    }
}

function onButtonToRemoveSearchTextClick() {
    searchResults.value = []
    searchBar.value!.value = ''
    loadingErrorMessage.value = undefined
}
</script>

<template>
    <div class="big-margin-container">
        <p class="text-field-label">Titel:</p>
        <input
            type="text"
            class="text-field"
            @input="onTitleInput"
            maxlength="200"
        >

        <p class="text-field-label">Beschreibung:</p>
        <textarea
            class="long-text-area"
            maxlength="10000"
            @input="onDescriptionAreaInput"
        ></textarea>

        <p v-if="addedMedias.length > 0" class="text-field-label">Filme und Serien auf der Watchlist:</p>

        <div v-for="media, index in addedMedias" class="film-or-serie-container">
            <md-ripple></md-ripple>
            <RouterLink
                v-if="media.media_type == 'movie' || media.media_type == 'tv'"
                :to="{ name: media.media_type == 'movie' ? 'film' : 'serie', params: { id: media.id }}"
                class="film-or-serie"
            >
                <img v-if="media.poster_path" :src="`https://image.tmdb.org/t/p/w92${media.poster_path}`" />
                <div class="media-text">
                    <h2 v-if="media.media_type == 'movie'">{{ media.title }}</h2>
                    <h2 v-else>{{ media.name }}</h2>
                    <p>{{ media.overview }}</p>
                </div>
                <button class="primary-button" @click="removeMedia(index, $event)" style="z-index: 100;">Löschen</button>
            </RouterLink>
        </div>

        <div id="search-bar-wrapper">
            <input
                id="search-bar"
                ref="watchlist-search-bar"
                placeholder="Films oder Series suchen"
                @input="onSearchBarInput"
                @keyup.enter="fetchFilmsAndSeries(($event.target as HTMLInputElement).value)"
            >
            <img
                v-if="showButtonToRemoveSearchText"
                src="@/assets/images/icons/close.svg"
                alt="Remove search text"
                id="remove-search-text-icon"
                @click="onButtonToRemoveSearchTextClick"
            >
        </div>

        <LoadingWrapper :loaded-ref="searchResults" :error-message="loadingErrorMessage">
            <p v-if="searchResults!.length > 0" class="text-field-label">Suchergebnisse:</p>
            <div v-for="media, index in searchResults" class="film-or-serie-container">
                <!-- TODO Gérer tabindex -->
                <md-ripple></md-ripple>
                <RouterLink
                    v-if="media.media_type == 'movie' || media.media_type == 'tv'"
                    :to="{ name: media.media_type == 'movie' ? 'film' : 'serie', params: { id: media.id }}"
                    class="film-or-serie"
                >
                    <img v-if="media.poster_path" :src="`https://image.tmdb.org/t/p/w92${media.poster_path}`" />
                    <div class="media-text">
                        <h2 v-if="media.media_type == 'movie'">{{ media.title }}</h2>
                        <h2 v-else>{{ media.name }}</h2>
                        <p>{{ media.overview }}</p>
                    </div>
                    <button class="primary-button" @click="addMedia(index, $event)" style="z-index: 100;">Hinzufügen</button>
                </RouterLink>
            </div>
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

<style lang="scss">
// todo fix this

.film-or-serie-container {
	position: relative;
	border-radius: 20px;
}

.film-or-serie {
	display: flex;
	gap: 15px;
	padding: 15px;
	-webkit-align-items: center;
	align-items: center;
	color: white;
	-webkit-text-decoration: none;
	text-decoration: none;
	overflow: hidden;
	max-height: 250px;
	width: 100%;

    .media-text {
        flex-grow: 6;
    }

	p {
		display: -webkit-box;
		line-clamp: 2;
		-webkit-line-clamp: 2;
		-webkit-box-orient: vertical;
		overflow: hidden;
		text-overflow: ellipsis;
		max-height: 250px;
		width: 100%;
	}

	img {
		border-radius: 10px;
		width: 100px;
	}
}

#add-button, l-dot-spinner {
    display: block;
    margin: 20px auto 0 auto;
}

#search-bar-wrapper {
    position: relative;

    #remove-search-text-icon {
        position: absolute;
        right: 7px;
        top: 7px;
        cursor: pointer;
    }
}
</style>
