<script setup lang="ts">
import { RouterView, RouterLink } from 'vue-router'
import { MdFilledTonalButton } from '@material/web/button/filled-tonal-button';
</script>

<template>
  <header>
    <RouterLink tabindex="-1" to="/"> <md-filled-tonal-button> Home </md-filled-tonal-button> </RouterLink>
    <RouterLink tabindex="-1" to="/films-series"> <md-filled-tonal-button> Films und Series </md-filled-tonal-button> </RouterLink>
    <RouterLink tabindex="-1" to="/watch-lists"> <md-filled-tonal-button> Watch lists </md-filled-tonal-button> </RouterLink>
    <RouterLink tabindex="-1" to="/log-in"> <md-filled-tonal-button> Log in </md-filled-tonal-button> </RouterLink>
    <RouterLink tabindex="-1" to="/sign-up"> <md-filled-tonal-button> Sign up </md-filled-tonal-button> </RouterLink>
  </header>

  <main>
    <RouterView />
  </main>

  <footer>
  </footer>
</template>


<style lang="scss">
header {
	height: 80px;
	background-color: var(--color-background-mute);
	display: flex;
	justify-content: center;
	gap: 10px;
	-webkit-align-items: center;
	align-items: center;

	--md-filled-tonal-button-label-text-size: 1.15em;
	--md-filled-tonal-button-label-text-line-height: 45px;
	--md-filled-tonal-button-container-color: transparent;
	--md-filled-tonal-button-label-text-color: white;
	--md-filled-tonal-button-hover-label-text-color: white;
	--md-filled-tonal-button-focus-label-text-color: white;

	> * {
		color: white;
	}

	.router-link-active {
		--md-filled-tonal-button-container-color: rgba(255, 255, 255, 0.2);
	}
}

main {
	flex-grow: 1;
	background-color: var(--color-background);
}
</style>