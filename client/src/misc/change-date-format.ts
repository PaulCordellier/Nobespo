export default function tmdbDateFromatToGermanDate(dateString : string) : string  {
    let year = dateString.slice(0, 4)
    let month = dateString.slice(5, 7)
    let day = dateString.slice(8, 10)

    return day + "." + month + "." + year
}