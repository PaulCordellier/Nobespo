export type Watchlist = {
    title: string
    description: string
    filmsIds: number[]
    seriesIds: number[]
}

export type WatchlistInfo = Watchlist & {
    username: string,
    publishDate: Date,
    id: number,
    posterPaths: string[]
}