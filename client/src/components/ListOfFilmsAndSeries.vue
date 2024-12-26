<script setup lang="ts">
import { RouterLink } from "vue-router"
import { MdRipple } from "@material/web/ripple/ripple"
defineProps<{
    medias?: any[]
}>()
</script>

<template>
    <div class="film-and-series-list" v-if="medias && medias.length > 0">
        <div v-for="media in medias" class="film-or-serie-container">
            <md-ripple></md-ripple>
            <RouterLink
                v-if="media.media_type == 'movie' || media.media_type == 'tv'"
                :to="{ name: media.media_type == 'movie' ? 'film' : 'serie', params: { id: media.id }}"
                class="film-or-serie"
            >
                <img v-if="media.poster_path" :src="`https://image.tmdb.org/t/p/w154${media.poster_path}`" />
                <div>
                    <h2 v-if="media.media_type == 'movie'">{{ media.title }}</h2>
                    <h2 v-else>{{ media.name }}</h2>
                    <p>{{ media.overview }}</p>
                </div>
            </RouterLink>
        </div>
    </div>
</template>

<!--
possible poster sizes : w92, w154, w185, w342, w500, w780, original
-->

<style lang="scss">
.film-and-series-list {
	width: 100%;
}

.film-or-serie-container {
	position: relative;
	border-radius: 20px;
}

.film-or-serie {
	display: flex;
	gap: 15px;
	padding: 15px;
	-webkit-align-items: center;
	align-items: center;
	color: white;
	-webkit-text-decoration: none;
	text-decoration: none;
	overflow: hidden;
	max-height: 250px;
	width: 100%;

	p {
		display: -webkit-box;
		line-clamp: 6;
		-webkit-line-clamp: 6;
		-webkit-box-orient: vertical;
		overflow: hidden;
		text-overflow: ellipsis;
		max-height: 250px;
		width: 100%;
	}

	img {
		border-radius: 10px;
		width: 150px;
	}
}
</style>