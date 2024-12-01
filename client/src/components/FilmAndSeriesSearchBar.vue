<script setup lang="ts">
import { useTemplateRef } from 'vue'
import { useRouter, useRoute } from "vue-router"

// TODO rename and add properties to this component so it also searches for watch lists

const router = useRouter()
const route = useRoute()
const textField = useTemplateRef('search-bar')

function onEnterPressed() {
    let writtenText = textField.value!.value

    if (!textField || writtenText.length <= 0) {
        return
    }
    
    let query = writtenText
    query = encodeURI(query)
    
    router.push({ name: 'search-films-series', params: { query }})
}
</script>

<template>
        <input
            placeholder="Films oder Series suchen"
            id="search-bar"
            ref="search-bar"
            type="search"
            :value="route.params.query ? decodeURI(route.params.query as string) : ''"
            @keyup.enter="onEnterPressed"
            class="text-field"
        >
</template>

<style lang="scss">
#search-bar {
    padding: 15px;
    margin: 20px 20%;
    padding-left: 48px;
    width: 60%;

    background: url(@/assets/images/icons/search.svg) no-repeat scroll 8px 9px;
    background-size: 35px;
}

@media (max-width: 700px) {
    #search-bar {
        padding: 20px 10%;
    }
}
</style>