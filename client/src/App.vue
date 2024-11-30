<script setup lang="ts">
import { RouterView, RouterLink } from 'vue-router'
import { MdFilledTonalButton } from '@material/web/button/filled-tonal-button'
import { MdMenu } from '@material/web/menu/menu'
import { MdMenuItem } from '@material/web/menu/menu-item'
import { useCurrentUserStore } from "@/stores/currentUser"
import { useRouter } from "vue-router"

const currentUserStore = useCurrentUserStore()

const router = useRouter()

function showAccountMenu() {
    const accountMenu = document.getElementById("account-menu") as MdMenu
    accountMenu.open = !accountMenu.open; 
}

function disconnect() {
    currentUserStore.disconnectUser()
    router.push("/")
}
</script>

<template>
  <header>
    <RouterLink tabindex="-1" to="/"> <md-filled-tonal-button> Home </md-filled-tonal-button> </RouterLink>
    <RouterLink tabindex="-1" to="/films-series"> <md-filled-tonal-button> Films und Series </md-filled-tonal-button> </RouterLink>
    <RouterLink tabindex="-1" to="/watch-lists"> <md-filled-tonal-button> Watch lists </md-filled-tonal-button> </RouterLink>
 
    <md-filled-tonal-button v-if="currentUserStore.isConnected" @click="showAccountMenu" id="account-button">
        <div id="account-button-content">
            <img src="@/assets/images/icons/default-user.png">
            <p>{{ currentUserStore.username }}</p>
        </div>
        <md-menu id="account-menu" anchor="account-button">
            <md-menu-item>
                <div slot="headline">Kontoeinstellungen</div> <!-- TODO -->
            </md-menu-item>
            <md-menu-item @click="disconnect">
                <div slot="headline">Sich abmelden</div>
            </md-menu-item>
        </md-menu>
    </md-filled-tonal-button>
    <RouterLink tabindex="-1" to="/log-in" v-if="!currentUserStore.isConnected"> <md-filled-tonal-button> Log in </md-filled-tonal-button> </RouterLink>
    <RouterLink tabindex="-1" to="/sign-up" v-if="!currentUserStore.isConnected"> <md-filled-tonal-button> Sign up </md-filled-tonal-button> </RouterLink>
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

#account-button-content {
    display: flex;
    position: relative;
    align-items: center;
    column-gap: 10px;

    height: 60px;
    border-radius: 30px;

    img {
        width: 40px;
        height: 40px;
    }
}
</style>