import { Footer } from "@lobehub/ui";
import {
    description,
    email
} from '../../../package.json'

export default function ThorFooter() {
    return (
        <Footer
            bottom={
                <div>
                    <p>© {new Date().getFullYear()} AIDotNet. All rights reserved.
                    </p>
                    <p>
                        Powered by .NET 9
                    </p>
                </div>
            }
            columns={[
                {
                    title: '关于我们',
                    items: [
                        {
                            title: '关于我们',
                            description: description,
                        },
                        {
                            title: '邮箱',
                            description: <a href={"mailto://" + email}>联系我们</a>
                        }
                    ]
                },
                {
                    title: '其他开源项目',
                    items: [
                        {
                            title: 'AIDotNet',
                            description: <a href="https://github.com/AIDotNet"
                                target="_blank"
                            >
                                AIDotNet社区是一群热爱AI的开发者组成的社区，
                                <br />
                                旨在推动AI技术的发展，为AI开发者提供更好的学习和交流平台。
                            </a>
                        },
                        {
                            title: 'FastWiki',
                            description: <a href="https://github.com/AIDotNet/Fast-wiki"
                                target="_blank"
                            >
                                一个智能知识库的开源项目，
                                <br />
                                可用于开发企业级智能客服管理系统。
                            </a>
                        },
                        {
                            title: 'linker',
                            description: <a href="https://github.com/snltty/linker"
                                target="_blank"
                            >
                                一个免费开源的异地组网工具
                            </a>
                        }
                    ]
                }
            ]}
        >

        </Footer>
    )
}