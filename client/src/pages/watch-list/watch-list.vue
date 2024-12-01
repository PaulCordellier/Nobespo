<script setup lang="ts">
import { ref } from 'vue';
import { MdOutlinedButton } from '@material/web/button/outlined-button'

import CommentSection from "@/components/CommentSection.vue"
import StarsRatingHover from '@/components/rating/StarsRatingHover.vue'
import FilmAndSeriesList from '@/components/FilmAndSeriesList.vue'

// I'm adding dummy text for testing, but when the api will be ready I'll delete that
let filmAndSeries = ref<any[]>([{"backdrop_path": "/sYXLeu5usz6yEz0k00FYvtEdodD.jpg","id": 94605,"name": "Arcane","original_name": "Arcane","overview": "Im Konflikt zwischen den Städten Piltover und Zhaun stehen zwei Schwestern auf entgegengesetzten Seiten eines Krieges um magische Technologien und Überzeugungen.","poster_path": "/abf8tHznhSvl9BAElD2cQeRr7do.jpg","media_type": "tv","adult": false,"original_language": "en","genre_ids": [16,10765,10759,9648],"popularity": 1095.249,"first_air_date": "2021-11-06","vote_average": 8.8,"vote_count": 4453,"origin_country": ["US"]},{"backdrop_path": "/o002wVtLqO1ZPBV3svRVvLqy8wc.jpg","id": 45275,"name": "Arcana Famiglia: La storia della Arcana Famiglia","original_name": "アルカナ・ファミリア","overview": "Die Geschichte handelt von Arcana Famiglia, einer selbsternannten Organisation mit mysteriösen Kräften, die sie dafür nutzen, um eine mediterrane Insel vor Piraten, fremden Ländern und anderen Bedrohungen zu beschützen. Felicitá, die Tochter des Anführers, soll mit dem neuen Anführer, der in einem Wettbewerb in zwei Monaten, an dem auch Felicitá teilnimmt, bestimmt wird, heiraten.","poster_path": "/iAG03eJ32d6HaDdR7MnEvEyZpYr.jpg","media_type": "tv","adult": false,"original_language": "ja","genre_ids": [16,10759,35,18],"popularity": 25.29,"first_air_date": "2012-07-01","vote_average": 6.3,"vote_count": 17,"origin_country": ["JP"]},{"backdrop_path": "/fNB96NjSNKTX1fHdId3L6L64t8p.jpg","id": 43071,"title": "Corto Maltese: The Secret Court of the Arcane","original_title": "Corto Maltese : La Cour secrète des Arcanes","overview": "At the end of 1918 while civil war is raging on in Russia, antagonism is slowly spreading to the East, between the Oral mountains and Shanghai. Stuck between a desire to save what's left of the great Imperial Russia, and starting from a clean slate, old generals, secret organizations, and mercenaries attracted by gold, struggle to take advantage of the events. As Corto Maltese returns to Shanghai, he barely gets time to cross paths with his old friend/nemesis Rasputin, and escape a murder attempt before being contacted by members of a Chinese secret organization called \"The Red Lanterns\". In the heart of violent Manchurian horizons, Corto and Rasputin launch themselves into a fabulous treasure hunt, following the tracks of the mysterious armor-plated train of Kolchak. A steel monster spiked with canons and machine guns, this trains protects the counter-revolutionaries gold...","poster_path": "/tSY0FtnDCu40xEQuhOwpbdFXM7p.jpg","media_type": "movie","adult": false,"original_language": "fr","genre_ids": [18,16,36,12],"popularity": 15.387,"release_date": "2002-09-25","video": false,"vote_average": 6.4,"vote_count": 45},{"backdrop_path": "/xdO32Ph1j0eFNZP4WWYO64trUUA.jpg","id": 143451,"title": "Arcana","original_title": "Arcana","overview": "In einem Armutsviertel am Rande Mailands lebt die verwitwete Signora Tarantino (Lucia Bosé), eine Lateinamerikanerin, mit ihrem Sohn (Maurizio degli Esposti) in einem schäbigen, verkommenen Haus. Die Mutter hofft, der Armutsfalle zu entkommen, indem sie Seancen gibt, vornehmlich wohlhabenden Kunden, denen sie das Geld aus den Taschen schwindelt. Ihr Sohn verachtet sie dafür, empfindet aber auch generell nur Abscheu für seine Umwelt. An den Seancen nimmt er jedoch mit halber Belustigung als \"Geist\" teil und zu seiner Mutter unterhält er wie selbstverständlich eine inzestuöse Beziehung. Insgeheim ist er fasziniert von der schwarzen Magie und entschlossen, ihrem wahren Geheimnis auf den Grund zu gehen. Als die naive Marisa (Tina Aumont) seine Mutter wegen ihrer bevorstehenden Heirat konsultiert und um einen Talisman bittet, erhält er die Gelegenheit, seine Künste zu erproben...","poster_path": "/rkd1iV0PddE8MG6bjYovx4uN8sR.jpg","media_type": "movie","adult": false,"original_language": "it","genre_ids": [18,27,9648],"popularity": 4.186,"release_date": "1972-04-06","video": false,"vote_average": 5.7,"vote_count": 10},])
</script>

<template>
    <div class="basic-padding-container">
        <div id="media-or-watchlist-full-description">
            <!-- <img :src="`https://image.tmdb.org/t/p/w185${???}`" id="poster"/> -->
            <div id="details">
                <p id="first-line">
                    <span id="title">Best of Post-apo</span>
                    <span class="separator"> • </span>
                    <span id="length-info"> 8 movies and series</span>
                    <span class="separator"> • </span>
                    <span id="release-date">Last update</span>
                </p>

                <p id="description">I like post apo</p>

                <p>Wie würden Sie diese Watchlist bewerten?</p>
                <StarsRatingHover starEnabledColor="white" starDisabledColor="grey"/>
                <md-outlined-button>Erstellen</md-outlined-button>
            </div>
        </div>
        <FilmAndSeriesList :medias="filmAndSeries"/>
        <CommentSection />
    </div>
</template>
