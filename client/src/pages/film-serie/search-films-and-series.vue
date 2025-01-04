<script setup lang="ts">
import SearchBar from "@/components/SearchBar.vue"
import ListOfFilmsAndSeries from "@/components/ListOfFilmsAndSeries.vue"
import LoadingWrapper from "@/components/LoadingWrapper.vue"

import { onMounted, watch, ref } from "vue";
import { useRoute } from "vue-router";

const route = useRoute()
const apiResults = ref<any[]>()
const loadingErrorMessage = ref<string | undefined>()

onMounted(fetchApi)

watch(
  () => route.params.query,
  fetchApi
)

async function fetchApi() {
    apiResults.value = undefined
    loadingErrorMessage.value = undefined

    const response = await fetch(`/api/tmdb/search-films-and-series/${route.params.query}`, { method: "GET" })

    if (response.ok) {
        const apiResponse = await response.json()
        apiResults.value = apiResponse.results as any[]
    } else {
        loadingErrorMessage.value = "Fehler: Code " + response.status
    }
}
</script>

<template>
    <div class="big-margin-container">
        <SearchBar searchRouteName="search-films-series" />
        <LoadingWrapper :loaded-ref="apiResults" :error-message="loadingErrorMessage">
            <ListOfFilmsAndSeries :medias="apiResults" />
            <h1 v-if="apiResults!.length == 0" class="error-message">Nothing was found !</h1>
        </LoadingWrapper>
    </div>
</template>

<style lang="scss">
.error-message {
    text-align: center;
}
</style>