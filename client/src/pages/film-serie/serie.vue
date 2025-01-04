<script setup lang="ts">
import { MdOutlinedButton } from '@material/web/button/outlined-button'

import { onMounted, ref } from "vue"
import { useRoute } from "vue-router"

import StarsRatingHover from '@/components/rating/StarsRatingHover.vue'
import CommentSection from "@/components/CommentSection.vue"
import LoadingWrapper from "@/components/LoadingWrapper.vue"

const route = useRoute()

const media = ref()
const seasons = ref<any[]>([])
const loadingErrorMessage = ref<string | undefined>()

onMounted(async () =>  {

    loadingErrorMessage.value = undefined

    const response = await fetch(`/api/tmdb/serie/${route.params.id}`, { method: "GET" })

    if (response.ok) {
        media.value = await response.json()
        seasons.value = media.value.seasons

        // We want to show the number of seasons, but sometimes in the database "extras" are counted as a season.
        // This code removes thoose extras.
        seasons.value = seasons.value.filter(item => {
            let seriesName: string = item.name.toLowerCase()
            return seriesName != "extras" && seriesName != "extra"
        })
    } else {
        loadingErrorMessage.value = "Fehler: Code " + response.status
    }
})
</script>

<template>
    <LoadingWrapper :loaded-ref="media" :error-message="loadingErrorMessage">
        <div class="big-margin-container">
            <div id="media-or-watchlist-full-description">
                <img :src="`https://image.tmdb.org/t/p/w185${media.poster_path}`" id="poster"/>
                <div id="details">
                    <p id="first-line">
                        <span id="title">{{ media.name }}</span>
                        <span class="separator"> • </span>
                        <span id="length-info" v-if="seasons.length <= 1"> {{ seasons.length }} Staffel</span>
                        <span id="length-info" v-else> {{ seasons.length }} Staffeln</span>
                        <span class="separator"> • </span>
                        <span id="release-date">Letzte Sendung: {{ new Date(media.first_air_date).toLocaleDateString('de-DE') }}</span>
                    </p>

                    <p id="description">{{ media.overview }}</p>

                    <p>Wie würden Sie diese Serie bewerten?</p>
                    <StarsRatingHover starEnabledColor="white" starDisabledColor="grey"/>

                    <md-outlined-button>Erstellen</md-outlined-button>
                </div>
            </div>
            <CommentSection
                :urlToGetComments="`/api/comment/get/serie/${media.id}`"
                :urlToPublishComments="`/api/comment/publish/serie/${media.id}`" />
        </div>
    </LoadingWrapper>
</template>
