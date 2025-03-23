<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'

import StarsRating from '@/components/rating/StarsRating.vue'
import RatingDialog from '@/components/rating/RatingDialog.vue'
import CommentSection from '@/components/CommentSection.vue'
import LoadingWrapper from '@/components/LoadingWrapper.vue'
import ListOfFilmsAndSeries from '@/components/ListOfFilmsAndSeries.vue'
import { type WatchlistInfo } from '@/misc/watchlist-types'
import { useCurrentUserStore } from '@/stores/currentUser'

const route = useRoute()

const watchlist = ref<WatchlistInfo>()
const filmAndSeries = ref<any[]>()
const loadingErrorMessage = ref<string>()

const currentUserStore = useCurrentUserStore()
const userRating = ref<number>(0)

onMounted(async () => {

    loadingErrorMessage.value = undefined

    let response = await fetch(`/api/watchlist/getbyid/${route.params.id}`, { method: "GET" })

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
        
        response = await fetch(filmOrSerieUrl, { method: "GET" })

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

    if (!currentUserStore.isConnected) {
        return
    }

    response = await fetch("/api/rate/watchlist/" + route.params.id, {
        method: "GET",
        headers: {
            'Authorization': `Bearer ${currentUserStore.token}`,
        }
    })

    console.log(response.status);
    

    if (response.ok) {
        userRating.value = await response.json() as number
    }
})
</script>

<template>
    <LoadingWrapper :loadedRef="watchlist" :errorMessage="loadingErrorMessage">
        <div class="basic-margin-container">
            <div id="media-or-watchlist-full-description">
                <div id="details">
                    <p id="first-line">
                        <span id="title">{{ watchlist!.title }}</span>

                        <span class="separator"> • </span>

                        <span v-if="watchlist!.filmsIds.length == 0" id="length-info"> {{ watchlist!.seriesIds.length }} Series</span>
                        <span v-else-if="watchlist!.seriesIds.length == 0" id="length-info"> {{ watchlist!.filmsIds.length }} Films</span>
                        <span v-else id="length-info"> {{ watchlist!.filmsIds.length + watchlist!.seriesIds.length }} Films und Series</span>

                        <span class="separator"> • </span>

                        <span id="release-date">gepostet am {{ new Date(watchlist!.publishDate).toLocaleDateString('de-DE') }}</span>
                    </p>
    
                    <p id="description">{{ watchlist!.description }}</p>

                    <br v-if="userRating != 0">
                    <p v-if="userRating != 0">Ihre Bewertung</p>
                    <StarsRating
                        v-if="userRating != 0"
                        starEnabledColor="white"
                        starDisabledColor="grey"
                        :rating="userRating"
                        :size="40"
                    />

                    <br>
    
                    <RatingDialog
                        starEnabledColor="white"
                        starDisabledColor="grey"
                        dialog-title="Wie würden Sie diese Watchlist bewerten?"
                        :media-id="parseInt(route.params.id as string)"
                        post-url="/api/rate/watchlist/"
                        @rating-changed="$router.go(0)"
                    />
                    
                    <ListOfFilmsAndSeries :medias="filmAndSeries" :shouldBeSmall="true"/>
                </div>
            </div>
            <CommentSection
                :urlToGetComments="`/api/comment/get/watchlist/${route.params.id}`"
                :urlToPublishComments="`/api/comment/publish/watchlist/${route.params.id}`"
            />
        </div>
    </LoadingWrapper>
</template>
