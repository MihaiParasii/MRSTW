export interface CreateDealRequest {
	title: string
	description: string
	categoryId: number | null
	subcategoryId: number | null
	regionId: number | null
}