declare module server {
	interface YoRequest {
		message: string;
	}
	interface YoResponse {
		allYos: number;
		userYos: number;
	}
	interface YoHistoryResponse {
		history: string[];
	}
}
