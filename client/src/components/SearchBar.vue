<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'

const { placeholder, searchRouteName, actionOnEnterPressed, onSearchTextRemoved, getSearchTextFromQuery } = defineProps<{
    placeholder: string
    searchRouteName?: string
    actionOnEnterPressed?: (searchText: string) => void
    onSearchTextRemoved?: () => void
    getSearchTextFromQuery?: boolean
}>()

const router = useRouter()
const route = useRoute()
const searchText = ref<string>('')

onMounted(() => {
    if (getSearchTextFromQuery) {
        searchText.value = decodeURI(route.params.query as string)
    }
})

function onSearchBarInput() {
    if (searchText.value == '' && onSearchTextRemoved) {
        onSearchTextRemoved()
    }
}

function onEnterPressed() {
    if (searchText.value == '') {
        return
    }

    if (searchRouteName) {
        router.push({ name: searchRouteName, params: { query: encodeURI(searchText.value) }})
    }

    if (actionOnEnterPressed) {
        actionOnEnterPressed(searchText.value)
    }
}

function onButtonToRemoveSearchTextClick() {
    searchText.value = ''
    
    if (onSearchTextRemoved) {
        onSearchTextRemoved()
    }
}
</script>

<template>
    <div id="search-bar-wrapper">
        <input
            id="search-bar"
            v-model="searchText"
            type="search"
            :placeholder="placeholder"
            @input="onSearchBarInput"
            @keyup.enter="onEnterPressed"
        >
        <button
            v-if="searchText != ''"
            id="remove-search-text-button"
            @click="onButtonToRemoveSearchTextClick"
        >
            <img src="@/assets/images/icons/close.svg" alt="Remove search text">
        </button>
    </div>
</template>

<style lang="scss">
@use '../assets/scss/text-fields.scss';

#search-bar {
    @extend .text-field;

    margin: 20px 0;
    padding-left: 48px;

    background: url(@/assets/images/icons/search.svg) no-repeat scroll 7px 7px;
    background-size: 35px;
}

@media (max-width: 700px) {
    #search-bar {
        padding: 10px 10%;
    }
}

#search-bar-wrapper {
    position: relative;

    #remove-search-text-button {
        position: absolute;
        cursor: pointer;
        background: none;
        border: none;
        right: 7px;
        top: 7px;
        padding: 0;
    }
}
</style>