<script setup lang="ts">
import { MdRipple } from "@material/web/ripple/ripple"
import { type WatchlistInfo } from "@/misc/watchlist-types"

defineProps<{
	watchlists?: WatchlistInfo[]
}>()
</script>

<template>
<RouterLink :to="`/watch-lists/1${watchlist.id}`" class="watchlist-tile" v-for="watchlist in watchlists">
    <md-ripple></md-ripple>
    <div>
        <div v-for="posterPath in watchlist.posterPaths" class="poster-wrapper">
            <img :src="`https://image.tmdb.org/t/p/w154${posterPath}`" class="poster">
        </div>
    </div>
    <div class="tile-text">
        <h2>{{ watchlist.title }}</h2>
        <p class="tile-paragraph">{{ watchlist.description }}</p>
    </div>
</RouterLink>
</template>


<style lang="scss">
@use '../assets/scss/tile.scss';

.watchlist-tile {
    @extend .tile;

    $poster-basic-width: 120px;
    $poster-basic-height: 170px;
    $poster-offset: 40px;

    .poster-wrapper {
        display: inline-block;
        position: relative;
        height: $poster-basic-height;
    }

    .poster-wrapper:nth-last-child(1) {
	    width: $poster-basic-width;
    }

    .poster-wrapper:nth-last-child(n + 2) {
        width: $poster-offset;

        .poster {
            filter: drop-shadow(10px 0 3px rgba(0, 0, 0, 0.5));
        }
    }

    .poster {
        @extend .poster-tile;

        display: block;
        position: absolute;
        height: unset;
        width: unset;
        left: 0;
        max-width: $poster-basic-width;
        max-height: $poster-basic-height;
    }

    $poster-offset: 25px;
    
    .poster-wrapper:nth-child(1) {
        z-index: 5;
    }

    .poster-wrapper:nth-child(2) {
        z-index: 4;
    }

    .poster-wrapper:nth-child(3) {
        z-index: 3;
    }

    .poster-wrapper:nth-child(4) {
        z-index: 2;
    }
}
</style>