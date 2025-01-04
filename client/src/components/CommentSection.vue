<script setup lang="ts">
import { onMounted, ref, useTemplateRef } from "vue"
import { useRouter } from "vue-router"
import { useCurrentUserStore } from "@/stores/currentUser"
import onLongTextAreaInput from "@/misc/onLongTextAreaInput"
import ButtonWithLoading from "@/components/ButtonWithLoading.vue"
import { ResponseState } from "@/components/ButtonWithLoading.vue"

const currentUserStore = useCurrentUserStore()
const router = useRouter()

const commentTextArea = useTemplateRef("comment-text-area")

const { urlToGetComments, urlToPublishComments } = defineProps<{
    urlToGetComments: string
    urlToPublishComments: string
}>()

type Comment = {
    text: string
    username: string
    publishDate: string
}

const comments = ref<Comment[] | null>(null)
const responseState = ref<ResponseState>(ResponseState.NoRequest)

onMounted(getComments)

async function getComments() {

    const response = await fetch(urlToGetComments, { method: 'GET' })

    if (response.ok) {
        comments.value = await response.json() as Comment[]
        responseState.value = ResponseState.NoRequest
    } else {
        // TODO
    }
}

const showPostButton = ref<boolean>(false)

function onCommentTextAreaInput(event : Event) {
    let commentTextArea = event.target as HTMLTextAreaElement
    onLongTextAreaInput(commentTextArea)
    showPostButton.value = commentTextArea.value != ''
}

async function publishComment() {
    responseState.value = ResponseState.Loading

    const response = await fetch(urlToPublishComments, {
        method: 'POST',
        headers: {
            'Authorization': `Bearer ${currentUserStore.token}`,
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(commentTextArea.value?.value)
    })

    if (response.ok) {
        await getComments()
        responseState.value = ResponseState.NoRequest
        commentTextArea.value!.value = ''
        showPostButton.value = false
    } else if (response.status == 401 || (response.body && await response.json() == 'Bad token')) {
        currentUserStore.disconnectUser()
        router.push({ name: 'login' })
    } else {
        responseState.value = ResponseState.Error
    }
}
</script>

<template>
    <div id="comments">
        <h1>Kommentare:</h1>
        <div class="comment">
            <div class="info-line-comment">
                <img src="@/assets/images/icons/default-user.png" />
                <p>{{ currentUserStore.isConnected ? currentUserStore.username : "user name here" }}</p>
            </div>
            <textarea
                class="long-text-area"
                ref="comment-text-area"
                placeholder="Schreiben Sie Ihren Kommentar hier!"
                maxlength="10000"
                contenteditable="true"
                @input="onCommentTextAreaInput"
            ></textarea>

            <ButtonWithLoading
                v-if="showPostButton" 
                :button-event="publishComment" 
                button-text="Posten"
                :response-state="responseState" />
        </div>
        <!-- TODO use LoadingWrapper here -->
        <div class="comment" v-if="comments && comments.length >= 1" v-for="comment in comments">
            <div class="info-line-comment">
                <img src="@/assets/images/icons/default-user.png" />
                <p>{{ comment.username }}</p>
                <p class="stars">★★★★</p>
            </div>
            <p>{{ comment.text }}</p>
        </div>
        <div class="comment" v-if="comments && comments.length == 0">
            <p>Es gibt noch keine Kommentare.</p>
        </div>
    </div>
</template>


<style lang="scss">
#comments {
	padding-top: 15px;
}

.comment {
	padding: 10px 0;
}

.info-line-comment {
	display: flex;
	gap: 10px;
	-webkit-align-items: center;
	align-items: center;
    
	img {
		-moz-border-radius: 50%;
		-webkit-border-radius: 50%;
		border-radius: 50%;
        width: 50px;
        height: 50px;
	}
}

.stars {
	color: green;
}
</style>