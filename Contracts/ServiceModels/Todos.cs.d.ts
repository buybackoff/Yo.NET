declare module server {
	/** REST Resource DTO */
	interface Todos {
		ids: number[];
	}
	interface Todo {
		id: number;
		content: string;
		order: number;
		done: boolean;
	}
}
