<script setup lang="ts">
import FilmsAndSeriesSearchBar from "@/components/FilmAndSeriesSearchBar.vue"
import FilmsAndSeriesList from "@/components/FilmAndSeriesList.vue"
import { onMounted, ref } from "vue";

const apiResultsLeftSide = ref<any[]>()
const apiResultsRightSide = ref<any[]>()

onMounted(fetchTrending)

async function fetchTrending() {
    
    const response = await fetch("/api/tmdb/trending", { method: "GET" })

    if (response.ok) {
        const apiResponse = await response.json()

        const apiResults = apiResponse.results as any[]
        const middleIndex = Math.ceil(apiResults.length / 2)

        apiResultsLeftSide.value = apiResults.slice(0, middleIndex)
        apiResultsRightSide.value = apiResults.slice(middleIndex)
    }
}

</script>

<template>
    <FilmsAndSeriesSearchBar />

    <div id="trending" v-if="apiResultsLeftSide && apiResultsRightSide">
        <FilmsAndSeriesList :medias="apiResultsLeftSide" />
        <FilmsAndSeriesList :medias="apiResultsRightSide" />
    </div>
</template>


<style lang="scss">
#trending {
    display: flex;
    flex-direction: row;
    gap: 10px;
    padding: 0 10%;
    padding-bottom: 40px;
}

@media (max-width: 900px) {
    #trending {
        flex-direction: column;
        gap: 0;
        padding: 0 10px;
        padding-bottom: 40px;
    }
}
</style>