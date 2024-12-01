<script setup lang="ts">
import StarSvg from '@/components/rating/StarSvg.vue'

defineProps<{
    rating: number
    starEnabledColor: string
    starDisabledColor: string
}>()
</script>

<template>

<div class="stars" ref="starsRatingWrapper">
    <StarSvg
        v-for="number in [ 0, 1, 2, 3, 4 ]"
        class="star"
        :leftSideColor="(rating / 2) - number > 0 ? starEnabledColor : starDisabledColor"
        :rightSideColor="(rating / 2) - number - 0.5 > 0 ? starEnabledColor : starDisabledColor"
        :size="40"
    />
</div>

</template>

<style lang="scss">
.stars {
    display: flex;
    flex-direction: row;
    justify-content: space-between;
    width: 240px;
}
</style>