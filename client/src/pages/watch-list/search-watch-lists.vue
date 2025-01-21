<script setup lang="ts">
import { ref, onMounted, watch } from "vue"
import { useRoute } from "vue-router"
import SearchBar from "@/components/SearchBar.vue"
import ListOfWatchlists from "@/components/ListOfWatchlists.vue"
import type { WatchlistInfo } from "@/misc/watchlist-types"
import LoadingWrapper from "@/components/LoadingWrapper.vue"

const route = useRoute()
const apiResults = ref<WatchlistInfo[]>()
const loadingErrorMessage = ref<string | undefined>()

onMounted(fetchApi)

watch(
  () => route.params.query,
  fetchApi
)

async function fetchApi() {
    apiResults.value = undefined
    loadingErrorMessage.value = undefined

    const response = await fetch(`/api/watchlist/search/${route.params.query}`, { method: "GET" })

    if (response.ok) {
        const apiResponse = await response.json()
        apiResults.value = apiResponse as WatchlistInfo[]
    } else {
        loadingErrorMessage.value = "Fehler: Code " + response.status
    }
}
</script>

<template>
    <div class="basic-margin-container">
        <SearchBar
            placeholder="Watchlists suchen"
            search-route-name="search-watch-lists"
            :get-search-text-from-query="true"
        />
        
        <LoadingWrapper :loaded-ref="apiResults" :error-message="loadingErrorMessage">
            <ListOfWatchlists :watchlists="apiResults"/>
            <h1 v-if="apiResults?.length === 0" class="error-message">Nothing was found !</h1>
        </LoadingWrapper>
    </div>
</template>
