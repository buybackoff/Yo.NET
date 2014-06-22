declare module server {
	/** Request DTO */
	interface Yo {
		name: string;
		message: string;
		withHistory: boolean;
	}
	/** TODO service to get all messages, paginated, use infinite scroll on teh client sideResponse DTO */
	interface YoResponse {
		allYos: number;
		myYos: number;
		history: string[];
		responseStatus: {
			errorCode: string;
			errors: any[];
			message: string;
			stackTrace: string;
		};
	}
}
