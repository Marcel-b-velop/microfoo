import { Subject } from "rxjs";

export function sharedFunction() {
  console.log("sharedFunction called");
}

export interface State {
  message: string;
  token: string;
  // eslint-disable-next-line no-unused-vars
  shouldNavigate: (sender: string) => boolean;
}

export const getStore = (): Subject<State> => new Subject<State>();
export type Store = Subject<State>;
export const getShouldNavigate = () => new Subject<ShouldNavigate>();
// eslint-disable-next-line no-unused-vars
export type ShouldNavigate = (sender: string) => boolean;
