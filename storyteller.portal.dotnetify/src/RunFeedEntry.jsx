import React from 'react';
import dotnetify from 'dotnetify';

class RunFeedEntry extends React.Component {
    constructor(props) {
        super(props);
        //dotnetify.react.connect("RunFeed", this);
        this.state = { RunSummary: props.RunSummary };
    }
    render() {
        return (<tr key={this.state.RunSummary.Id}>
                    <td>{this.state.RunSummary.Id}</td>
                    <td>{this.state.RunSummary.RunDateTime}</td>
            <td>{this.state.RunSummary.AppName}</td>
            <td>{this.state.RunSummary.Name}</td>
            <td>(Pass: {this.state.RunSummary.SuccessfulCount} Fail: {this.state.RunSummary.FailureCount} Total: {this.state.RunSummary.TotalCount})</td>
            </tr >);
    }
}
export default RunFeedEntry;