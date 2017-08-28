import { fetch, addTask } from 'domain-task';
import { Action, Reducer, ActionCreator } from 'redux';
import { AppThunkAction } from './';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.

export interface AppsState {
    isLoaded: boolean;
    apps: App[];
}

export interface App {
    id: number;
    name: string;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

interface RequestAppsAction {
    type: 'REQUEST_APPS';
}

interface ReceiveAppsAction {
    type: 'RECEIVE_APPS';
    apps: App[];
}

// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = RequestAppsAction | ReceiveAppsAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

export const actionCreators = {
    requestApps: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
        
        
        let state = getState().apps;
        console.log(state.isLoaded);
        
        if (!state.isLoaded) {      
            let fetchTask = fetch(`Apps`)
                .then(response => response.json() as Promise<App[]>)
                .then(data => {                    
                    dispatch({ type: 'RECEIVE_APPS', apps: data });                    
                });

            addTask(fetchTask); // Ensure server-side prerendering waits for this to complete
            dispatch({ type: 'REQUEST_APPS' });
        }
    }
};

const unloadedState: AppsState = { apps: [], isLoaded: false };

export const reducer: Reducer<AppsState> = (state: AppsState, incomingAction: Action) => {
    const action = incomingAction as KnownAction;
    
    switch (action.type) {
        case 'REQUEST_APPS':
            return {
                apps: state.apps,
                isLoaded: false
            };
        case 'RECEIVE_APPS':
            // Only accept the incoming data if it matches the most recent request. This ensures we correctly
            // handle out-of-order responses.
            //if (state.isLoaded) {
                return {
                    apps: action.apps,
                    isLoaded: true
                };
            //}
            //break;
        default:
            // The following line guarantees that every action in the KnownAction union has been covered by a case above
            const exhaustiveCheck: never = action;
    }

    return state || unloadedState;
};
