<script setup lang="ts">
import { RouterLink } from "vue-router"
import { MdRipple } from "@material/web/ripple/ripple"

defineProps<{
    medias: any[] | undefined,
	buttonInfo?: {
		buttonText: string,
		buttonEvent: (index: number, event: Event) => void
	}
	shouldBeSmall?: boolean
}>()
</script>

<template>
<div>
	<div v-for="media, index in medias">
		<RouterLink
			v-if="media.media_type == 'movie' || media.media_type == 'tv'"
			:to="{ name: media.media_type == 'movie' ? 'film' : 'serie', params: { id: media.id }}"
			class="tile"
		>
			<md-ripple></md-ripple>
			<img
				v-if="media.poster_path"
				:src="`https://image.tmdb.org/t/p/${shouldBeSmall ? 'w92' : 'w154'}${media.poster_path}`"
				:class="shouldBeSmall ? 'poster-tile-smaller' : 'poster-tile'"
			/>
			<div class="tile-text">
				<h2 class="tile-title" v-if="media.media_type == 'movie'">{{ media.title }}</h2>
				<h2 class="tile-title" v-else>{{ media.name }}</h2>
				<p :class="shouldBeSmall ? 'tile-paragraph-smaller' : 'tile-paragraph'">{{ media.overview }}</p>
			</div>
			<button
				v-if="buttonInfo"
				@click="event => buttonInfo!.buttonEvent(index, event)"
				class="primary-button"
			>
				{{ buttonInfo!.buttonText }}
			</button>
		</RouterLink>
	</div>
</div>
</template>

<!--
possible poster sizes : w92, w154, w185, w342, w500, w780, original
-->