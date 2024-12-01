<script setup lang="ts">
import { MdRipple } from "@material/web/ripple/ripple"

// TODO add properties here
</script>

<template>
<RouterLink to="/watch-lists/1" class="watchlist-button" v-for="number in [1, 2, 3, 4]">
    <md-ripple></md-ripple>
    <!-- TODO add the posters here -->
    <div>
        <h2>Best of Post-apo</h2>
        <p>I like post apo</p>
    </div>
</RouterLink>
</template>


<style lang="scss">
.watchlist-button {
	position: relative;
	border-radius: 20px;
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
}
</style>