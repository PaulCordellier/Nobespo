export type Watchlist = {
    title: string
    description: string
    filmsIds: number[]
    seriesIds: number[]
}

export type WatchlistInfo = Watchlist & {
    username: string,
    publishDate: string,
    id: number,
    posterPaths: string[]
}