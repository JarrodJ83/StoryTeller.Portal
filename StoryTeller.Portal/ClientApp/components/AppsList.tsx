import * as React from 'react';
import { Link, RouteComponentProps } from 'react-router-dom';
import { connect } from 'react-redux';
import { ApplicationState } from '../store';
import * as AppsState from '../store/Apps';

// At runtime, Redux will merge together...
type AppsProps =
    AppsState.AppsState   // ... state we've requested from the Redux store
    & typeof AppsState.actionCreators      // ... plus action creators we've requested
    & RouteComponentProps<{ }>; // ... plus incoming routing parameters

class AppsList extends React.Component<AppsProps, {}> {
    componentWillMount() {
        this.props.requestApps();
    }

    componentWillReceiveProps(nextProps: AppsProps) {
        this.props.requestApps();
    }

    public render() {
        return <div>
            <h1>Applications:</h1>  
            {this.renderAppsTable()}
        </div>;
    }

    private renderAppsTable() { 
        return <table className='table'>
            <thead>
                <tr>
                    <th>Application</th>
                </tr>
            </thead>
            <tbody>
                {this.props.apps.map(a =>                    
                    <tr key={a.id}>
                        <td>{a.name}</td>
                    </tr>
                )}  
            </tbody>
        </table>;
    }
}

export default connect(
    (state: ApplicationState) => state.apps, AppsState.actionCreators                
)(AppsList) as typeof AppsList;