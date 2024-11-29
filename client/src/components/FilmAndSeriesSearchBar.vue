<script setup lang="ts">
import { MdOutlinedTextField } from '@material/web/textfield/outlined-text-field';
import { MdIcon } from '@material/web/icon/icon';       // TODO Avoid using material design
import { onMounted } from 'vue';
import { useRouter, useRoute } from "vue-router"

const router = useRouter()
const route = useRoute()
let textField : MdOutlinedTextField

onMounted(() => {
    textField = document.getElementById("search-bar") as MdOutlinedTextField
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
  <md-outlined-text-field
    placeholder="Films oder Series suchen"
    id="search-bar"
    type="search"
    :value="route.params.query ? decodeURI(route.params.query as string) : ''"
    @submit=""
    @keyup.enter="onEnterPressed"
  >
    <md-icon slot="leading-icon">Search</md-icon>
  </md-outlined-text-field>
</template>

<style lang="scss">
#search-bar {
    padding: 20px 20%;
    width: 100%;
}

@media (max-width: 700px) {
    #search-bar {
        padding: 20px 10%;
    }
}
</style>