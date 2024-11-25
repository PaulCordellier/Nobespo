<script setup lang="ts">
import FilmsAndSeriesSearchBar from "@/components/FilmAndSeriesSearchBar.vue"
import FilmsAndSeriesList from "@/components/FilmAndSeriesList.vue"
import { onMounted, watch, ref } from "vue";
import { useRoute } from "vue-router";

const route = useRoute()
const apiResults = ref<any[]>()

onMounted(fetchApi)

watch(
  () => route.params.query,
  fetchApi
)

async function fetchApi() {
  apiResults.value = undefined
  let response = await fetch(`/api/tmdb/search-films-and-series/${route.params.query}`)

  if (response.ok) {
    let apiResponse = await response.json()
    apiResults.value = apiResponse.results as any[]
  }
}
</script>

<template>
  <FilmsAndSeriesSearchBar />
  <FilmsAndSeriesList :medias="apiResults" class="basic-padding-container"/>
  <h1 v-if="apiResults && apiResults.length == 0" class="error-message">Nothing was found !</h1>
</template>

<style lang="scss">
.error-message {
    text-align: center;
}
</style>