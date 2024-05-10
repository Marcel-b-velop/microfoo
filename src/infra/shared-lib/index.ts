import { Subject } from 'rxjs'

export function sharedFunction () {
  console.log('sharedFunction called')
}

export interface State {
  message: string;
  token: string;
  shouldNavigate: (sender: string) => boolean;
}

export const getStore = (): Subject<State> => new Subject<State>()
export type Store = Subject<State>;
export const getShouldNavigate = () => new Subject<ShouldNavigate>();
export type ShouldNavigate = (sender: string) => boolean;
