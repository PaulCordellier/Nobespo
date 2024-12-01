<script setup lang="ts">
import { MdOutlinedButton } from '@material/web/button/outlined-button'

import CommentSection from "@/components/CommentSection.vue"

import { onMounted, ref } from "vue"
import { useRoute } from "vue-router"

import tmdbDateFromatToGermanDate from "@/misc/change-date-format"
import StarsRatingHover from '@/components/rating/StarsRatingHover.vue'

const route = useRoute()

const media = ref()

onMounted(fetchTrending)

async function fetchTrending() {
    
    const response = await fetch(`/api/tmdb/movie/${route.params.id}`, { method: "GET" })

    if (response.ok) {
        media.value = await response.json()
    }
}
</script>

<template>
    <div v-if="media" class="basic-padding-container">
        <div id="media-or-watchlist-full-description">
            <img :src="`https://image.tmdb.org/t/p/w185${media.poster_path}`" id="poster"/>
            <div id="details">
                <p id="first-line">
                    <span id="title">{{ media.title }}</span>
                    <span class="separator" v-if="media.runtime != 0"> • </span>
                    <span id="length-info" v-if="media.runtime != 0">{{ media.runtime }} mins</span>
                    <span class="separator"> • </span>
                    <span id="release-date">{{ tmdbDateFromatToGermanDate(media.release_date) }}</span>
                </p>

                <p id="description">{{ media.overview }}</p>

                <p>Wie bewerten Sie diesen Film?</p>
                <StarsRatingHover starEnabledColor="white" starDisabledColor="grey"/>

                <md-outlined-button>Erstellen</md-outlined-button>
            </div>
        </div>
        <CommentSection />
    </div>
</template>
