import { notFound } from 'next/navigation'
import { findLeaf } from '@/lib/navigation'
import ListEditPage from '@/components/templates/ListEditPage'
import TransactionDocumentPage from '@/components/templates/TransactionDocumentPage'
import ReportCriteriaPage from '@/components/templates/ReportCriteriaPage'
import WizardPage from '@/components/templates/WizardPage'
import ToolPage from '@/components/templates/ToolPage'

interface Props {
  params: Promise<{ slug: string[] }>
}

export default async function CatchAllPage({ params }: Props) {
  const { slug } = await params
  const slugPath = slug.join('/')
  const leaf = findLeaf(slugPath)

  if (!leaf) notFound()

  switch (leaf.type) {
    case 'list-edit':
      return <ListEditPage title={leaf.title} desktop={leaf.desktop} />
    case 'transaction':
      return <TransactionDocumentPage title={leaf.title} desktop={leaf.desktop} />
    case 'report':
      return <ReportCriteriaPage title={leaf.title} desktop={leaf.desktop} />
    case 'wizard':
      return <WizardPage title={leaf.title} desktop={leaf.desktop} />
    case 'tool':
    default:
      return <ToolPage title={leaf.title} desktop={leaf.desktop} description={leaf.description} />
  }
}
