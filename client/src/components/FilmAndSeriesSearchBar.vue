<script setup lang="ts">
import { onMounted } from 'vue';
import { useRouter, useRoute } from "vue-router"

const router = useRouter()
const route = useRoute()

let textField : HTMLInputElement

onMounted(() => {
    textField = document.getElementById("search-bar") as HTMLInputElement
})

function onEnterPressed() {
    if (!textField || textField.value.length <= 0) {
        return
    }
    
    let query = textField.value
    query = encodeURI(query)
    
    router.push({ name: 'search-films-series', params: { query }})
}
</script>

<template>
        <input
            placeholder="Films oder Series suchen"
            id="search-bar"
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