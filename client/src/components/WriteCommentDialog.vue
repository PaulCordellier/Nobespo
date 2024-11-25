<script setup lang="ts">
  import '@material/web/button/outlined-button.js';
  import '@material/web/checkbox/checkbox.js';
  import '@material/web/dialog/dialog.js';
  import '@material/web/textfield/filled-text-field.js';
</script>

<template>
  <md-dialog id="write-comment-dialog" style="margin: auto; width: 400px;">

    <div slot="headline">Kommentar schreiben</div>

    <form slot="content" id="form-id" method="dialog">
      <br />
      <p class="stars" style="font-size: 1.5em;">★★★★★</p>
      <br />
      <md-filled-text-field type="textarea"
                            label="Kommentar"
                            rows="3"
                            style="resize: vertical; width: 100%">
      </md-filled-text-field>
      <br />
      <br />
      <label>
        <md-checkbox touch-target="wrapper"></md-checkbox>
        Es gibt Spoiler
      </label>
    </form>

    <div slot="actions">
      <md-outlined-button form="form-id" value="cancel">Abbrechen</md-outlined-button>
      <md-outlined-button form="form-id" value="ok">Publizieren</md-outlined-button>
    </div>

  </md-dialog>
</template>
