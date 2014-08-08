declare module server {
	interface YoRequest {
		name: string;
		message: string;
		withHistory: boolean;
	}
	interface YoResponse {
		allYos: number;
		myYos: number;
		history: string[];
	}
	interface YoCounterResponse {
		totalCounter: number;
		userCounter: number;
	}
}
