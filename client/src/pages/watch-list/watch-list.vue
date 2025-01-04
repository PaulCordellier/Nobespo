<script setup lang="ts">
import { ref, onMounted } from "vue"
import { useRoute } from "vue-router"

import { MdOutlinedButton } from "@material/web/button/outlined-button"

import CommentSection from "@/components/CommentSection.vue"
import StarsRatingHover from "@/components/rating/StarsRatingHover.vue"
import ListOfFilmsAndSeries from "@/components/ListOfFilmsAndSeries.vue"
import LoadingWrapper from "@/components/LoadingWrapper.vue"
import { type WatchlistInfo } from "@/misc/watchlist-types"

const route = useRoute()

let watchlist = ref<WatchlistInfo>()
let filmAndSeries = ref<any[]>()
let loadingErrorMessage = ref<string>()

onMounted(async () => {

    loadingErrorMessage.value = undefined

    const response = await fetch(`/api/watchlist/getbyid/${route.params.id}`, { method: "GET" })

    if (response.ok) {
        watchlist.value = await response.json() as WatchlistInfo
    } else {
        loadingErrorMessage.value = "Fehler: Code " + response.status
        return
    }

    // For now we just have the ids of the series and the films of the watchlist. But we want to
    // access the data of the the series and the films and put it in the array filmAndSeries.value

    const filmsIds = watchlist.value.filmsIds
    const seriesIds = watchlist.value.seriesIds

    const filmsAndSeriesUrls: string[] = filmsIds.map(filmId => `/api/tmdb/movie/${filmId}`)
                                                 .concat(seriesIds.map(serieId => `/api/tmdb/serie/${serieId}`))

    filmAndSeries.value = []
    
    for (let filmOrSerieUrl of filmsAndSeriesUrls) {
        
        const response = await fetch(filmOrSerieUrl, { method: "GET" })

        if (response.ok) {
            let filmOrSerie = await response.json()

            if (filmOrSerieUrl.includes("movie")) {
                filmOrSerie.media_type = "movie"
            } else {
                filmOrSerie.media_type = "tv"
            }

            filmAndSeries.value.push(filmOrSerie)
        } else {
            loadingErrorMessage.value = "Fehler: Code " + response.status
            return
        }
    }
})
</script>

<template>
    <LoadingWrapper :loadedRef="watchlist" :errorMessage="loadingErrorMessage">
        <div class="big-margin-container">
            <div id="media-or-watchlist-full-description">
                <!-- <img :src="`https://image.tmdb.org/t/p/w185${???}`" id="poster"/> -->
                <div id="details">
                    <p id="first-line">
                        <span id="title">{{ watchlist!.title }}</span>

                        <span class="separator"> • </span>

                        <span v-if="watchlist!.filmsIds.length == 0" id="length-info"> {{ watchlist!.seriesIds.length }} Series</span>
                        <span v-else-if="watchlist!.seriesIds.length == 0" id="length-info"> {{ watchlist!.filmsIds.length }} Films</span>
                        <span v-else id="length-info"> {{ watchlist!.filmsIds.length + watchlist!.seriesIds.length }} Films und Series</span>

                        <span class="separator"> • </span>

                        <span id="release-date">gepostet am {{ watchlist!.publishDate.toLocaleDateString('de-DE') }}</span>
                    </p>
    
                    <p id="description">{{ watchlist!.description }}</p>
    
                    <p>Wie würden Sie diese Watchlist bewerten?</p>
                    <StarsRatingHover starEnabledColor="white" starDisabledColor="grey"/>
                    <ListOfFilmsAndSeries :medias="filmAndSeries" :shouldBeSmall="true"/>
                    <md-outlined-button>Erstellen</md-outlined-button>
                </div>
            </div>
            <CommentSection
                :urlToGetComments="`/api/comment/get/watchlist/${route.params.id}`"
                :urlToPublishComments="`/api/comment/publish/watchlist/${route.params.id}`"
            />
        </div>
    </LoadingWrapper>
</template>
