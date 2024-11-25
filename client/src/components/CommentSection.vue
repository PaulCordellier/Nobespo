<script setup lang="ts">
// TODO
</script>

<template>
    <div id="comments">
        <h1>Kommentar</h1>
        <div class="comment">
            <div class="info-line-comment">
                <img src="https://s.ltrbxd.com/static/img/avatar48.4cc5919f.png" />
                <p>user415642</p>
                <p class="stars">★★★</p>
            </div>
            <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident.</p>
        </div>
        <div class="comment">
            <div class="info-line-comment">
                <img src="https://s.ltrbxd.com/static/img/avatar48.4cc5919f.png" />
                <p>highiquser</p>
                <p class="stars">★★★★</p>
            </div>
            <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident.</p>
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
	}
}

.stars {
	color: green;
}
</style>