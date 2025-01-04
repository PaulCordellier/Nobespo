<script setup lang="ts">
import SearchBar from "@/components/SearchBar.vue"
import ListOfFilmsAndSeries from "@/components/ListOfFilmsAndSeries.vue"
import LoadingWrapper from "@/components/LoadingWrapper.vue"
import { onMounted, ref } from "vue"

const apiResultsLeftSide = ref<any[]>()
const apiResultsRightSide = ref<any[]>()

const loadingErrorMessage = ref<string | undefined>()

onMounted(fetchTrending)

async function fetchTrending() {

    loadingErrorMessage.value = undefined
    
    const response = await fetch("/api/tmdb/trending", { method: "GET" })

    if (response.ok) {
        const apiResponse = await response.json()

        const apiResults = apiResponse.results as any[]
        const middleIndex = Math.ceil(apiResults.length / 2)

        apiResultsLeftSide.value = apiResults.slice(0, middleIndex)
        apiResultsRightSide.value = apiResults.slice(middleIndex)
    } else {
        loadingErrorMessage.value = "Fehler: Code " + response.status
    }
}

</script>

<template>
    <div class="small-margin-container">
        <SearchBar searchRouteName="search-films-series" />
        <LoadingWrapper :loadedRef="apiResultsLeftSide" :errorMessage="loadingErrorMessage">
            <div id="trending">
                <ListOfFilmsAndSeries :medias="apiResultsLeftSide" />
                <ListOfFilmsAndSeries :medias="apiResultsRightSide" />
            </div>
        </LoadingWrapper>
    </div>
</template>


<style lang="scss" scoped>

#trending {
    display: flex;
    flex-direction: row;
    padding-bottom: 40px;

    div {
        width: 50%;
    }
}

@media (max-width: 900px) {
    #trending {
        flex-direction: column;
        padding-bottom: 40px;

        div {
            width: 100%;
        }
    }
}
</style>