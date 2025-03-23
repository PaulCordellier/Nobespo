<script setup lang="ts">
import { ref, useTemplateRef } from 'vue'

import StarsRating from '@/components/rating/StarsRating.vue'
import ButtonWithLoading, { ResponseState } from '@/components/ButtonWithLoading.vue'
import { useCurrentUserStore } from "@/stores/currentUser"

const starsRatingWrapper = useTemplateRef('starsRatingWrapper')
const currentUserStore = useCurrentUserStore()

const { postUrl, mediaId } = defineProps<{
    postUrl: string
    mediaId: number
    starEnabledColor: string
    starDisabledColor: string
    dialogTitle: string
}>()

const emit = defineEmits<{
    (e: 'ratingChanged'): void
}>()

const rating = ref<number>(0)
const showDialog = ref<boolean>(false)
const responseState = ref<ResponseState>(ResponseState.NoRequest)

function onMouseMoveOnRating(event: MouseEvent) {
    let clientBoundingRect = starsRatingWrapper.value!.$el.getBoundingClientRect()

    let xPositionInWrapper = event.x - clientBoundingRect.x

    let ratio = xPositionInWrapper / clientBoundingRect.width

    let currentRatingValue = Math.ceil(ratio * 10)

    if (rating.value != currentRatingValue) {
        rating.value = currentRatingValue
    }
}

async function postRating() {

    if (rating.value == 0) {
        return
    }

    responseState.value = ResponseState.Loading

    const response = await fetch(postUrl + mediaId, {
        method: 'POST',
        headers: {
            'Authorization': `Bearer ${currentUserStore.token}`,
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(rating.value)
    })
    
    if (response.ok) {
        showDialog.value = false
        responseState.value = ResponseState.NoRequest
        emit('ratingChanged')
    } else {
        responseState.value = ResponseState.Error
    }
} 
</script>

<template>

<button class="outlined-button" @click="showDialog = true">Bewerten</button>

<Transition>
    <div v-if="showDialog" class="rating-dialog-background" @click="showDialog = false">
        <div class="rating-dialog" @click.stop="">
            <h2>{{ dialogTitle }}</h2>
            <StarsRating
                :rating="rating"
                :star-enabled-color="starEnabledColor"
                :star-disabled-color="starDisabledColor"
                @mousemove="onMouseMoveOnRating"
                @click="postRating"
                ref="starsRatingWrapper"
                :size="40"
            />
            <div>
                <button class="outlined-button" @click="showDialog = false">Abbrechen</button>
                <ButtonWithLoading
                    :button-event="postRating"
                    button-text="Bestätigen"
                    :response-state="responseState"
                />
            </div>
        </div>
    </div>
</Transition>

</template>

<style lang="scss">
.rating-dialog-background {
    display: flex;
    justify-content: center;
    align-items: center;
    position: fixed;
    top: 0;
    bottom: 0;
    left: 0;
    right: 0;
    background-color: rgba(0, 0, 0, 0.3);
    z-index: 10;

    .rating-dialog {
        width: 500px;
        background-color: var(--color-background-mute);
        border-radius: 40px;
        padding: 20px;
        text-align: center;

        .stars {
            margin: 10px auto 17px auto;
        }

        .outlined-button {
            margin-right: 10px;
        }
    }
}

.v-enter-active,
.v-leave-active {
  transition: 0.2s ease;
}

.v-enter-from,
.v-leave-to {
  opacity: 0;   
}
</style>