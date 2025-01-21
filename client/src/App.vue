<script setup lang="ts">
import { ref, useTemplateRef } from 'vue'
import { RouterView, RouterLink } from 'vue-router'

import { useCurrentUserStore } from "@/stores/currentUser"

const currentUserStore = useCurrentUserStore()
const accountButton = useTemplateRef("account-button")

const showAccountMenu = ref<boolean>(false)

document.addEventListener('click', event => {
    // This code shows or hides the account-menu. If the user clicks elsewhere
    // while the account menu is shown, the account menu becomes hidden. The 
    // accountButton is the button that shows or hides the account menu.

    if (!showAccountMenu.value
        && (event.target as HTMLElement | null)?.id == accountButton.value?.id) {
        showAccountMenu.value = true
    }
    else if (showAccountMenu.value) {
        showAccountMenu.value = false
    }
})
</script>

<template>
<header>
    <RouterLink to="/" class="secondary-button"> Home </RouterLink>
    <RouterLink to="/films-series" class="secondary-button"> Films und Series </RouterLink>
    <RouterLink to="/watch-lists" class="secondary-button"> Watch lists </RouterLink>

    <div v-if="currentUserStore.isConnected" id="account-button" ref="account-button">
        <div id="account-button" class="bigger-secondary-button">
            <img src="@/assets/images/icons/default-user.png">
            <p>{{ currentUserStore.username }}</p>
            <Transition>
                <div v-if="showAccountMenu" id="account-menu">
                    <RouterLink :to="{ name: 'search-users' }">
                        Freund Hinzufügen
                    </RouterLink>
                    <RouterLink :to="{ name: 'create-watchlist' }">
                        Eine Watchlist erstellen
                    </RouterLink>
                    <RouterLink to="/" @click="currentUserStore.disconnectUser()">
                        Sich abmelden
                    </RouterLink>
                </div>
            </Transition>
        </div>
    </div>

    <RouterLink to="/log-in" v-if="!currentUserStore.isConnected" class="secondary-button"> Log in </RouterLink>
    <RouterLink to="/sign-up" v-if="!currentUserStore.isConnected" class="secondary-button"> Sign up </RouterLink>
</header>

<main>
    <RouterView />
</main>
</template>


<style lang="scss" scoped>
header {
	height: 80px;
	background-color: var(--color-background-mute);
	display: flex;
	justify-content: center;
	gap: 10px;
	-webkit-align-items: center;
	align-items: center;
}

main {
	flex-grow: 1;
	background-color: var(--color-background);
}

$account-menu-button-height: 50px;

#account-button {
    display: flex;
    position: relative;
    align-items: center;
    column-gap: 10px;

    height: 60px;
    border-radius: 30px;

    > img {
        width: 40px;
        height: 40px;
        pointer-events: none;
    }

    > p {
        pointer-events: none;
    }

    #account-menu {
        display: flex;
        position: absolute;
        flex-direction: column;
        top: 65px;
        left: -10px;
        background-color: var(--color-background-mute);
        border-radius: 20px;
        overflow: hidden;
        box-shadow: 0 0 10px black;
        z-index: 3;
        width: 200px;

        > a {
            line-height: $account-menu-button-height;
        }

        > * {
            padding: 0 20px;
        }

        > *:hover {
            transition: background-color 0.2s;
            background-color: rgba(255, 255, 255, 0.2);
        }
    }
}

.v-enter-active,
.v-leave-active {
  transition: 0.2s ease;
  height: calc($account-menu-button-height * 3);
}

.v-enter-from,
.v-leave-to {
  opacity: 0;
  height: 0;
}
</style>