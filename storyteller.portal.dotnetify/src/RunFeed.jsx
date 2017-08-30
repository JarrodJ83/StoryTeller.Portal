import React from 'react';
import dotnetify from 'dotnetify';

class RunFeed extends React.Component {
    constructor(props) {
        super(props);
        dotnetify.react.connect("RunFeed", this);
        this.state = { Runs:[] };
    }
    getInitialState() {
        this.vm.onRouteEnter = (path, template) => template.Target = "HtmlResults";
    }
    render() {
        return (
            <div className="App-intro">
            <table>
            <thead>
            <tr>
                <th>Run Date</th>
                <th>App</th>
                <th>Status</th>
                <th>Results</th>
            </tr>
            </thead>
            <tbody>
                        {this.state.Runs.map(run => <tr key={run.Id}>
                                                        <td>{run.Name}</td>
                                                        <td>{run.AppName}</td>
                                                        <td>(Pass: {run.SuccessfulCount} Fail: {run.FailureCount} Total: {run.TotalCount})</td>
                                                        <td>
                                <a target="_blank" href={"http://localhost:1881/Runs/" + run.Id + "/results"} style={{visibility: run.Finished ? "visible" : "hidden"}} >View Results</a>
                                                        </td>
                                                    </tr >)}
                    </tbody>
            </table>                 
            </div>
        );
    }
}
export default RunFeed;