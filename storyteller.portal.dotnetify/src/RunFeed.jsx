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
                    <div>{run}</div>
                )}
            </div>
        );
    }
}
export default RunFeed;