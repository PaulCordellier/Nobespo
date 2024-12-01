<script setup lang="ts">
import StarsRating from '@/components/rating/StarsRating.vue'
import { ref, useTemplateRef } from "vue"

const starsRatingWrapper = useTemplateRef('starsRatingWrapper')

defineProps<{
    starEnabledColor: string
    starDisabledColor: string
}>()

let rating = ref<number>(0)

const emit = defineEmits<{
    (e: 'hoverRatingValue', ratingValue : number): void
}>()
            // TODO : Use define expose or define emits?
defineExpose({
    rating
})


function onMouseMove(event: MouseEvent) {
    let clientBoundingRect = starsRatingWrapper.value!.$el.getBoundingClientRect()

    let xPositionInWrapper = event.x - clientBoundingRect.x

    let ratio = xPositionInWrapper / clientBoundingRect.width

    let currentRatingValue = Math.ceil(ratio * 20) / 2

    if (rating.value != currentRatingValue) {
        rating.value = currentRatingValue
        emit('hoverRatingValue', currentRatingValue)
    }
}

</script>

<template>  

<StarsRating
    :rating="rating"
    :star-enabled-color="starEnabledColor"
    :star-disabled-color="starDisabledColor"
    @mousemove="onMouseMove"
    ref="starsRatingWrapper"
/>

</template>
