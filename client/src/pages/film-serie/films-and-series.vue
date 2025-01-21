<script setup lang="ts">
import { onMounted, ref } from "vue"
import SearchBar from "@/components/SearchBar.vue"
import ListOfFilmsAndSeries from "@/components/ListOfFilmsAndSeries.vue"
import LoadingWrapper from "@/components/LoadingWrapper.vue"

const apiResults = ref<any[]>()

const loadingErrorMessage = ref<string>()

onMounted(async () => {
    loadingErrorMessage.value = undefined
    
    const response = await fetch("/api/tmdb/trending", { method: "GET" })

    if (response.ok) {
        const apiResponse = await response.json()
        apiResults.value = apiResponse.results
    } else {
        loadingErrorMessage.value = "Fehler: Code " + response.status
    }
})
</script>

<template>
    <div class="small-margin-container">
        <SearchBar placeholder="Films oder Series suchen" searchRouteName="search-films-series" />
        <LoadingWrapper :loadedRef="apiResults" :errorMessage="loadingErrorMessage">
            <div id="trending">
                <ListOfFilmsAndSeries :medias="apiResults" />
            </div>
        </LoadingWrapper>
    </div>
</template>


<style>
#trending {
    display: grid;
    grid-template-columns: 1fr 1fr;
    padding-bottom: 40px;
}

@media (max-width: 900px) {
    #trending {
        grid-template-columns: 1fr;
    }
}
</style>