import React from 'react';
import dotnetify from 'dotnetify';

class RunFeed extends React.Component {
    constructor(props) {
        super(props);
        dotnetify.react.connect("RunFeed", this);
        this.state = { Runs:[] };
    }
    render() {
        return (
            <div className="App-intro">
                Runs:
                {this.state.Runs.map(run => 
                    <div>{run.RunDateTime} :: {run.AppName} - {run.Name} - ({run.SuccessfulCount + run.FailureCount} / {run.TotalCount})</div>
                )}
            </div>
        );
    }
}
export default RunFeed;