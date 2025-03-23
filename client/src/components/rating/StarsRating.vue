<script setup lang="ts">
import StarSvg from '@/components/rating/StarSvg.vue'

defineProps<{
    rating: number
    starEnabledColor: string
    starDisabledColor: string
    size: number
}>()

</script>

<template>

<div class="stars" ref="starsRatingWrapper" :style="{ 'width': (size + 5) * 5 + 'px' }">
    <StarSvg
        v-for="number in [ 0, 1, 2, 3, 4 ]"
        class="star"
        :leftSideColor="(rating / 2) - number > 0 ? starEnabledColor : starDisabledColor"
        :rightSideColor="(rating / 2) - number - 0.5 > 0 ? starEnabledColor : starDisabledColor"
        :size
    />
</div>

</template>

<style lang="scss">
.stars {
    display: flex;
    flex-direction: row;
    justify-content: space-between;
}
</style>