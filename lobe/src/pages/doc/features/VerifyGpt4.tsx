import { Alert, Tag } from "@lobehub/ui";
import { Collapse } from 'antd';

export default function VerifyGpt4() {
    return <div style={{
        overflow: 'auto',
        height: 'calc(100vh - 120px)'
    }}>
        <Alert message=""
            type="warning"
            description="GPT系列模型都是训练于2021年，那时候还没有GPT3.5和GPT4。
            虽然版本迭代更新到了GPT4，但他们都是基于3.0模型的升级加强，模型依然只记得自己的版本是3.0，请按本站提供的方法进行验证GPT4。
            目前只有GPT4 preview系列的模型知识库是2023年，您可以提问：“您的知识库截止于什么时候？”即可验证是否是真正的preview系列模型"
            showIcon />

        <Collapse>
            <Collapse.Panel header="Q:鲁迅为什么暴打周树人？" key="1">
                <span >
                    <Tag color='blue'>GPT4</Tag>
                    <p> 表示鲁迅和周树人是同一个人。</p>
                </span>
                <br />
                <span >
                    <Tag color="red">GPT3.5</Tag>
                    <p> 会一本正经的胡说八道。</p>
                </span>
            </Collapse.Panel>
            <Collapse.Panel header="Q: 我爸妈结婚时为什么没有邀请我？" key="2">
                <span >
                    <Tag color='blue'>GPT4</Tag>
                    <p> 他们结婚时你还没出生。</p>
                </span>
                <br />
                <span >
                    <Tag color="red">GPT3.5</Tag>
                    <p>  他们当时认为你还太小，所以没有邀请你。</p>
                </span>
            </Collapse.Panel>
            <Collapse.Panel header="Q: What yesterday's today is tomorrow's?" key="3">
                <span >
                    <Tag color='blue'>GPT4</Tag>
                    <p> Past (前天)</p>
                </span>
                <br />
                <span >
                    <Tag color="red">GPT3.5</Tag>
                    <p>   Yesterday (昨天)</p>
                </span>
            </Collapse.Panel>
        </Collapse>
    </div>;
}