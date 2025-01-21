<script setup lang="ts">
import { ref } from "vue"

import SearchBar from "@/components/SearchBar.vue"
import LoadingWrapper from "@/components/LoadingWrapper.vue"

import { useCurrentUserStore } from "@/stores/currentUser"

type SearchResult = {
    username: string,
    followsCurrentUser: boolean
}

const currentUserStore = useCurrentUserStore()

const searchResults = ref<SearchResult[] | undefined>([])
const loadingErrorMessage = ref<string>()

let lastRequestSearchText : string = ''

async function searchUsernames(searchText: string) {
    searchResults.value = undefined
    loadingErrorMessage.value = undefined

    const response = await fetch(`/api/user/searchbyusername/${searchText}`, {
        method: 'GET',
        headers: { 'Authorization': `Bearer ${currentUserStore.token}` }
    })
    
    if (response.ok) {
        searchResults.value = await response.json()
        lastRequestSearchText = searchText
    } else {
        loadingErrorMessage.value = "Fehler: Code " + response.status
    }
}

function onSearchTextRemoved() {
    searchResults.value = []
}

async function followUser(username: string) {
    await fetch(`/api/user/follow/${username}`, {
        method: 'POST',
        headers: {
            'Authorization': `Bearer ${currentUserStore.token}`,
            'Content-Type': 'application/json',
        },
    })

    searchUsernames(lastRequestSearchText)
}
async function unfollowUser(username: string) {
    await fetch(`/api/user/unfollow/${username}`, {
        method: 'POST',
        headers: {
            'Authorization': `Bearer ${currentUserStore.token}`,
            'Content-Type': 'application/json',
        },
    })

    searchUsernames(lastRequestSearchText)
}
</script>

<template>
<div class="big-margin-container">
    <SearchBar
        placeholder="Benutzername suchen"
        :action-on-enter-pressed="searchUsernames"
        :on-search-text-removed="onSearchTextRemoved"
    />
    <LoadingWrapper :loaded-ref="searchResults" :error-message="loadingErrorMessage">
        <div v-for="searchResult of searchResults">
            <div class="user-tile">
                <h2>{{ searchResult.username }}</h2>
                <button
                    v-if="searchResult.followsCurrentUser"
                    class="primary-button"
                    @click="unfollowUser(searchResult.username)"
                >
                    Unfollow
                </button>
                <button
                    v-else
                    class="primary-button"
                    @click="followUser(searchResult.username)"
                >
                    Follow
                </button>

            </div>
        </div>
        <h3 v-if="searchResults?.length === 0">Kein Ergebnis</h3>
    </LoadingWrapper>
</div>
</template>

<style lang="scss">
.user-tile {
    display: flex;
    gap: 10px;
    padding: 10px 0;
    align-items: center;

    h2 {
        flex-grow: 5;
    }
}
</style>