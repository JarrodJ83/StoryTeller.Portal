import React from 'react';
import dotnetify from 'dotnetify';
import RunFeedEntry from './RunFeedEntry';

class RunFeed extends React.Component {
    constructor(props) {
        super(props);
        dotnetify.react.connect("RunFeed", this);
        this.state = { Runs:[] };
    }
    render() {
        return (
            <div className="App-intro">
            <table>
            <thead>
            <tr>
                <th>Id</th>
                <th>Run Date</th>
                <th>App</th>
                <th>Run Name</th>
                <th>Status</th>
                <th>Results</th>
            </tr>
            </thead>
            <tbody>
            {this.state.Runs.map(run => <tr key={run.Id}>
                                                        <td>{run.Id}</td>
                                                        <td>{run.RunDateTime}</td>
                                                        <td>{run.AppName}</td>
                                                        <td>{run.Name}</td>
                                                        <td>(Pass: {run.SuccessfulCount} Fail: {run.FailureCount} Total: {run.TotalCount})</td>
                                                        <td>
                                <a href={"viewResults?runId=" + run.Id} style={{visibility: run.Finished ? "visible" : "hidden"}} >View Results</a>
                                                        </td>
                                                    </tr >)}
                    </tbody>
                </table>                 
            </div>
        );
    }
            }
            export default RunFeed;