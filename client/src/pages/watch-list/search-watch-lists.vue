<script setup lang="ts">
import FilmsAndSeriesSearchBar from "@/components/FilmAndSeriesSearchBar.vue"
import ListOfWatchlists from "@/components/ListOfWatchlists.vue"
</script>

<template>
    <FilmsAndSeriesSearchBar />
    
    <div class="basic-padding-container"> <!-- add v-if here -->
        <ListOfWatchlists />
    </div>
</template>
