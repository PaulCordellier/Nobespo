<script setup lang="ts">
import { ref, onMounted } from "vue"
import SearchBar from "@/components/SearchBar.vue"
import ListOfWatchlists from "@/components/ListOfWatchlists.vue"
import type { WatchlistInfo } from "@/misc/watchlist-types"
import LoadingWrapper from "@/components/LoadingWrapper.vue"

const apiResults = ref<WatchlistInfo[]>()

const loadingErrorMessage = ref<string | undefined>()

onMounted(fetchRecentWatchlists)

async function fetchRecentWatchlists() {

    loadingErrorMessage.value = undefined

    const response = await fetch("/api/watchlist/recent", { method: "GET" })

    if (response.ok) {
        apiResults.value = await response.json() as WatchlistInfo[]
    } else {
        loadingErrorMessage.value = "Fehler: Code " + response.status
    }
}
</script>

<template>
    <div class="basic-margin-container">
        <SearchBar placeholder="Watchlists suchen" search-route-name="search-watch-lists"/>
        <LoadingWrapper :loaded-ref="apiResults" :error-message="loadingErrorMessage">
            <ListOfWatchlists :watchlists="apiResults" />
        </LoadingWrapper>
    </div>
</template>
