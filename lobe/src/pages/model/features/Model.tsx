import { useEffect, useState } from "react";
import { getUseModel } from "../../../services/ModelService";
import { Grid, } from "@lobehub/ui";
import { OpenAI } from "@lobehub/icons";
import { Card, Badge } from "antd";

export default function ModelPage() {
    const [models, setModels] = useState([]);

    useEffect(() => {
        getUseModel().then((res) => {
            setModels(res.data);
        });
    }, []);
    return <>
        <Grid style={{
            padding: 16,
            overflow: 'auto',
            height: 'calc(100vh - 120px)'
        }} width={'100%'} gap={16} maxItemWidth={80} rows={4}>
            {models.map((model: any) => {
                return model.hot ? <Badge count={"热"} >
                    <Card
                        hoverable
                        style={{
                            height: 100,
                        }} >
                        <div style={{
                            display: 'flex',
                            alignItems: 'center',
                        }}>
                            <OpenAI size={42} />
                            <span style={{
                                fontSize: 16,
                                fontWeight: 'bold',
                                marginTop: -10,
                                marginLeft: 24,
                                display: 'block'
                            }}>
                                {model.model}
                            </span>
                        </div>
                    </Card>
                </Badge> : <Card
                    hoverable
                    style={{ height: 100 }} >
                    <div style={{
                        display: 'flex',
                        alignItems: 'center',
                    }}>
                        <OpenAI size={42} />
                        <span style={{
                            fontSize: 16,
                            fontWeight: 'bold',
                            marginTop: -10,
                            marginLeft: 24,
                            display: 'block'
                        }}>
                            {model.model}
                        </span>
                    </div>
                </Card>

            })}
        </Grid>
    </>;
}