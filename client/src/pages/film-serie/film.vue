<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute } from 'vue-router'

import StarsRating from '@/components/rating/StarsRating.vue'
import RatingDialog from '@/components/rating/RatingDialog.vue'
import CommentSection from '@/components/CommentSection.vue'
import LoadingWrapper from '@/components/LoadingWrapper.vue'

const route = useRoute()

const media = ref()
const loadingErrorMessage = ref<string | undefined>()

onMounted(async () => {

    loadingErrorMessage.value = undefined

    const response = await fetch(`/api/tmdb/movie/${route.params.id}`, { method: "GET" })

    if (response.ok) {
        media.value = await response.json()
    } else {
        loadingErrorMessage.value = "Fehler: Code " + response.status
    }
})
</script>

<template>
    <LoadingWrapper :loaded-ref="media" :error-message="loadingErrorMessage">
        <div v-if="media" class="basic-margin-container">
            <div id="media-or-watchlist-full-description">
                <img :src="`https://image.tmdb.org/t/p/w185${media.poster_path}`" id="poster"/>
                <div id="details">
                    <p id="first-line">
                        <span id="title">{{ media.title }}</span>
                        <span class="separator" v-if="media.runtime != 0"> • </span>
                        <span id="length-info" v-if="media.runtime != 0">{{ media.runtime }} mins</span>
                        <span v-if="media.release_date" class="separator"> • </span>
                        <span v-if="media.release_date" id="release-date">Erscheinungsdatum: {{ new Date(media.release_date).toLocaleDateString('de-DE') }}</span>
                    </p>

                    <p id="description">{{ media.overview }}</p>

                    <RatingDialog
                        starEnabledColor="white"
                        starDisabledColor="grey"
                        dialog-title="Wie würden Sie diesen Film bewerten?"
                        :media-id="parseInt(route.params.id as string)"
                        post-url="/api/rate/film/"
                        @rating-changed="$router.go(0)"
                    />
                </div>
            </div>
            <CommentSection
                :urlToGetComments="`/api/comment/get/film/${media.id}`"
                :urlToPublishComments="`/api/comment/publish/film/${media.id}`" />
        </div>
    </LoadingWrapper>
</template>
